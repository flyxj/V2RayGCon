using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using ScintillaNET;

namespace V2RayGCon.Views
{
    public partial class FormConfiger : Form
    {
        JObject configTemplate, configEditing, configDefault;
        int perConfigComponetIndex, perServIndex;

        Func<string, string> I18N, resData;
        Dictionary<int, string> configCompoentTable, ssrMethodTable;
        Service.Setting settings;
        ScintillaNET.Scintilla tboxConfig;

        public FormConfiger()
        {
            InitializeComponent();

            I18N = Lib.Utils.I18N;
            resData = Lib.Utils.resData;

            settings = Service.Setting.Instance;

            InitScintilla();
            InitForm();

            this.Show();
            
        }

        void InitScintilla()
        {

            tboxConfig = new Scintilla();
            panelScintilla.Controls.Add(tboxConfig);

            tboxConfig.Dock = DockStyle.Fill;
            tboxConfig.WrapMode = WrapMode.None;
            tboxConfig.IndentationGuides = IndentView.LookBoth;

            var scintilla = tboxConfig;
            // Configure the JSON lexer styles
            scintilla.Styles[Style.Default].Font = "Consolas";
            scintilla.Styles[Style.Default].Size = 11;
            scintilla.Styles[Style.Json.Default].ForeColor = Color.Silver;
            scintilla.Styles[Style.Json.BlockComment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            scintilla.Styles[Style.Json.LineComment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            scintilla.Styles[Style.Json.Number].ForeColor = Color.Olive;
            scintilla.Styles[Style.Json.PropertyName].ForeColor = Color.Blue;
            scintilla.Styles[Style.Json.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
            scintilla.Styles[Style.Json.StringEol].BackColor = Color.Pink;
            scintilla.Styles[Style.Json.Operator].ForeColor = Color.Purple;
            scintilla.Lexer = Lexer.Json;

            // folding
            // Instruct the lexer to calculate folding
            scintilla.SetProperty("fold", "1");
            scintilla.SetProperty("fold.compact", "1");

            // Configure a margin to display folding symbols
            scintilla.Margins[2].Type = MarginType.Symbol;
            scintilla.Margins[2].Mask = Marker.MaskFolders;
            scintilla.Margins[2].Sensitive = true;
            scintilla.Margins[2].Width = 20;

            // Set colors for all folding markers
            for (int i = 25; i <= 31; i++)
            {
                scintilla.Markers[i].SetForeColor(SystemColors.ControlLightLight);
                scintilla.Markers[i].SetBackColor(SystemColors.ControlDark);
            }

            // Configure folding markers with respective symbols
            scintilla.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            scintilla.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            scintilla.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            scintilla.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            scintilla.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            scintilla.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            scintilla.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            scintilla.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        bool CheckValid()
        {
            var content = tboxConfig.Text;
            if (perConfigComponetIndex >= 10)
            {
                JArray jarry;
                try
                {
                    jarry = JArray.Parse(content);
                }
                catch
                {
                    return false;
                }
                return true;
            }

            JObject jobj;
            try
            {
                jobj = JObject.Parse(content);
            }
            catch
            {
                return false;
            }
            return true;
        }

        bool Confirm(string content)
        {
            var confirm = I18N("Confirm");
            var text = content;
            return Lib.Utils.Confirm(confirm, text);
        }

        void ShowConfigByIndex(int selIdx)
        {
            // Debug.WriteLine("showConfigById: " + selIdx);

            if (selIdx == 0)
            {
                tboxConfig.Text = configEditing.ToString();
                return;
            }

            var part = configEditing[configCompoentTable[selIdx]];
            if (part == null)
            {
                if (selIdx >= 10)
                {
                    part = new JArray();
                }
                else
                {
                    part = new JObject();
                }
                configEditing[configCompoentTable[selIdx]] = part;
            }
            tboxConfig.Text = part.ToString();
            UpdateElement();
        }

        void SaveContentChange()
        {
            var content = tboxConfig.Text;
            if (perConfigComponetIndex >= 10)
            {
                var jarr = JArray.Parse(content);
                configEditing[configCompoentTable[perConfigComponetIndex]] = jarr;
                return;
            }
            if (perConfigComponetIndex == 0)
            {
                configEditing = JObject.Parse(content);
                return;
            }
            configEditing[configCompoentTable[perConfigComponetIndex]] = JObject.Parse(content);
        }

        bool CheckContentChange()
        {
            var content = tboxConfig.Text;

            if (perConfigComponetIndex >= 10)
            {
                var jarr = JArray.Parse(content);
                return !(JToken.DeepEquals(configEditing[configCompoentTable[perConfigComponetIndex]], jarr));
            }

            var jobj = JObject.Parse(content);
            if (perConfigComponetIndex == 0)
            {
                return !(JToken.DeepEquals(configEditing, jobj));
            }
            return !(JToken.DeepEquals(configEditing[configCompoentTable[perConfigComponetIndex]], jobj));
        }

        private void cboxConfigPart_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = cboxConfigPart.SelectedIndex;
            // Debug.WriteLine("Select id:" + selIdx);

            if (selectedIndex != perConfigComponetIndex)
            {
                if (CheckValid())
                {
                    if (CheckContentChange())
                    {
                        if (Confirm(I18N("EditorSaveChange")))
                        {
                            SaveContentChange();
                        }

                    }
                    perConfigComponetIndex = selectedIndex;
                    ShowConfigByIndex(selectedIndex);
                }
                else
                {
                    if (Confirm(I18N("CannotParseJson")))
                    {
                        perConfigComponetIndex = selectedIndex;
                        ShowConfigByIndex(selectedIndex);
                    }
                    else
                    {
                        cboxConfigPart.SelectedIndex = perConfigComponetIndex;
                        return;
                    }
                }
            }

            UpdateElement();
        }

        void btnSaveModify_Click(object sender, EventArgs e)
        {
            string content = tboxConfig.Text;
            if (perConfigComponetIndex >= 10)
            {
                JArray jarry;
                try
                {
                    jarry = JArray.Parse(content);
                    configEditing[configCompoentTable[perConfigComponetIndex]] = jarry;
                }
                catch
                {
                    MessageBox.Show(I18N("Can not parse json! Please check you config."));
                }
                UpdateElement();
                return;
            }

            JObject jobj;
            try
            {
                jobj = JObject.Parse(content);
                if (perConfigComponetIndex == 0)
                {
                    configEditing = jobj;
                }
                else
                {
                    configEditing[configCompoentTable[perConfigComponetIndex]] = jobj;
                }
            }
            catch
            {
                MessageBox.Show(I18N("Can not parse json! Please check you config."));
            }
            UpdateElement();
        }

        void FillTextBox(TextBox tbox, string perfix, string key)
        {
            tbox.Text = Lib.Utils.GetStrFromJToken(configEditing, perfix + key);
        }

        void FillTextBox(TextBox tbox, string perfix, string keyIP, string keyPort)
        {
            string ip = Lib.Utils.GetStrFromJToken(configEditing, perfix + keyIP);
            string port = Lib.Utils.GetStrFromJToken(configEditing, perfix + keyPort);
            tbox.Text = string.Join(":", ip, port);
        }

        void UpdateElement()
        {
            
            string perfix;

            // vmess
            perfix = "outbound.settings.vnext.0.users.0.";
            FillTextBox(tboxVMessID, perfix, "id");
            FillTextBox(tboxVMessLevel, perfix, "level");
            FillTextBox(tboxVMessAid, perfix, "alterId");

            perfix = "outbound.settings.vnext.0.";
            FillTextBox(tboxVMessIPaddr, perfix, "address", "port");

            // SS outbound.settings.servers.0.
            perfix = "outbound.settings.servers.0.";
            FillTextBox(tboxSSREmail, perfix, "email");
            FillTextBox(tboxSSRPass, perfix, "password");
            FillTextBox(tboxSSRAddr, perfix, "address", "port");

            bool ssrOTA = Lib.Utils.GetBoolFromJToken(configEditing, perfix + "ota");
            cboxSSROTA.SelectedIndex = ssrOTA ? 1 : 0;

            string ssrMethod = Lib.Utils.GetStrFromJToken(configEditing, perfix + "method");
            cboxSSRMethod.SelectedIndex = 0;
            foreach (var item in ssrMethodTable)
            {
                if (item.Value.Equals(ssrMethod))
                {
                    cboxSSRMethod.SelectedIndex = item.Key;
                    break;
                }
            }

            // kcp ws tls
            perfix = "outbound.streamSettings.";
            FillTextBox(tboxKCPType, perfix, "kcpSettings.header.type");
            FillTextBox(tboxWSPath, perfix, "wsSettings.path");

            var security = Lib.Utils.GetStrFromJToken(configEditing, perfix+ "security");
            cboxStreamSecurity.SelectedIndex = security.Equals("tls") ? 1 : 0;
        }

        void InsertStreamSecurity()
        {
            string sec = cboxStreamSecurity.SelectedIndex == 1 ? "tls" : "";
            try
            {
                configEditing["outbound"]["streamSettings"]["security"] = sec;
            }
            catch { }
            Debug.WriteLine("FormConfiger: can not inset stream security!");
        }

        private void btnClearModify_Click(object sender, EventArgs e)
        {
            if (perConfigComponetIndex == 0)
            {
                tboxConfig.Text = configEditing.ToString();
            }
            else
            {
                tboxConfig.Text = configEditing[configCompoentTable[perConfigComponetIndex]].ToString();
            }
        }

        private void btnOverwriteServConfig_Click(object sender, EventArgs e)
        {
            if (CheckContentChange())
            {
                if (!Confirm(I18N("EditorDiscardChange")))
                {
                    return;
                }
            }

            string cfgString = configEditing.ToString();
            settings.ReplaceServer(Lib.Utils.Base64Encode(cfgString), perServIndex);
            MessageBox.Show(I18N("Done"));
        }


        private void btnLoadDefault_Click(object sender, EventArgs e)
        {

            if (perConfigComponetIndex == 0)
            {
                tboxConfig.Text = configDefault.ToString();
            }
            else
            {
                var part = configDefault[configCompoentTable[perConfigComponetIndex]];
                if (part != null)
                {
                    tboxConfig.Text = configDefault[configCompoentTable[perConfigComponetIndex]].ToString();
                }
                else
                {
                    MessageBox.Show(I18N("EditorNoExample"));
                }
                return;
            }
        }

        private void btnVMessInsertClient_Click(object sender, EventArgs e)
        {
            string id = tboxVMessID.Text;
            int level = Lib.Utils.Str2Int(tboxVMessLevel.Text);
            int aid = Lib.Utils.Str2Int(tboxVMessAid.Text);

            Lib.Utils.TryParseIPAddr(tboxVMessIPaddr.Text, out string ip, out int port);

            JToken o = configTemplate["vmessClient"];
            o["vnext"][0]["address"] = ip;
            o["vnext"][0]["port"] = port;
            o["vnext"][0]["users"][0]["id"] = id;
            o["vnext"][0]["users"][0]["alterId"] = aid;
            o["vnext"][0]["users"][0]["level"] = level;

            ModifyOutBoundSetting(o, "vmess");

        }

        void ModifyOutBoundSetting(JToken set, string protocol)
        {
            try
            {
                configEditing["outbound"]["settings"] = set;
            }
            catch
            {
                Debug.WriteLine("FormConfiger: can not insert outbound.setting");
            }
            try
            {
                configEditing["outbound"]["protocol"] = protocol;
            }
            catch
            {
                Debug.WriteLine("FormConfiger: can not insert outbound.protocol");
            }

            ShowConfigByIndex(perConfigComponetIndex);
        }

        private void btnSSRInsertClient_Click(object sender, EventArgs e)
        {

            Lib.Utils.TryParseIPAddr(tboxSSRAddr.Text, out string ip, out int port);

            JToken o = configTemplate["ssrClient"];
            o["servers"][0]["email"] = tboxSSREmail.Text;
            o["servers"][0]["address"] = ip;
            o["servers"][0]["port"] = port;
            o["servers"][0]["method"] = ssrMethodTable[cboxSSRMethod.SelectedIndex];
            o["servers"][0]["password"] = tboxSSRPass.Text;
            o["servers"][0]["ota"] = cboxSSROTA.SelectedIndex == 1;

            ModifyOutBoundSetting(o, "shadowsocks");
        }

        private void btnStreamInsertKCP_Click(object sender, EventArgs e)
        {
            try
            {
                configEditing["outbound"]["streamSettings"] = configTemplate["kcp"];
                configEditing["outbound"]["streamSettings"]["kcpSettings"]["header"]["type"] = tboxKCPType.Text;
            }
            catch { }
            InsertStreamSecurity();
            ShowConfigByIndex(perConfigComponetIndex);
        }

        private void btnStreamInsertWS_Click(object sender, EventArgs e)
        {
            try
            {
                configEditing["outbound"]["streamSettings"] = configTemplate["ws"];
                configEditing["outbound"]["streamSettings"]["wsSettings"]["path"] = tboxWSPath.Text;
            }
            catch { }
            InsertStreamSecurity();
            ShowConfigByIndex(perConfigComponetIndex);
        }

        private void btnStreamInsertTCP_Click(object sender, EventArgs e)
        {
            try
            {
                configEditing["outbound"]["streamSettings"] = configTemplate["tcp"];
            }
            catch { };
            InsertStreamSecurity();
            ShowConfigByIndex(perConfigComponetIndex);
        }

        private void btnVMessGenUUID_Click(object sender, EventArgs e)
        {
            tboxVMessID.Text = Guid.NewGuid().ToString();
        }

        private void btnInsertNewServ_Click(object sender, EventArgs e)
        {
            if (CheckContentChange())
            {
                if (!Confirm(I18N("EditorDiscardChange")))
                {
                    return;
                }
            }
            string cfgString = configEditing.ToString();
            settings.AddServer(Lib.Utils.Base64Encode(cfgString));
            MessageBox.Show(I18N("Done"));
        }

        private void cboxShowPassWord_CheckedChanged(object sender, EventArgs e)
        {
            if (cboxShowPassWord.Checked == true)
            {
                tboxSSRPass.PasswordChar = '\0';

            }
            else
            {
                tboxSSRPass.PasswordChar = '*';
            }
        }

        JObject LoadServerConfig()
        {
            JObject o = null;
            string b64str = settings.GetServer(settings.curEditingIndex);
            if (!string.IsNullOrEmpty(b64str))
            {
                o = JObject.Parse(Lib.Utils.Base64Decode(b64str));
            }

            if (o == null)
            {
                o = JObject.Parse(resData("config_min"));
                MessageBox.Show(I18N("EditorCannotLoadServerConfig"));
            }

            return o;
        }

        void InitForm()
        {
            configCompoentTable = new Dictionary<int, string>
            {
                { 0, "config.json"},
                { 1, "log"},
                { 2, "api"},
                { 3, "dns"},
                { 4, "stats"},
                { 5, "routing"},
                { 6, "policy"},
                { 7, "inbound"},
                { 8, "outbound"},
                { 9, "transport"},
                { 10,"inboundDetour"},
                { 11,"outboundDetour"},

            };

            ssrMethodTable = new Dictionary<int, string>
            {
                { 0,"aes-128-cfb"},
                { 1,"aes-128-gcm"},
                { 2,"aes-256-cfb"},
                { 3,"aes-256-gcm"},
                { 4,"chacha20"},
                { 5,"chacha20-ietf"},
                { 6,"chacha20-poly1305"},
                { 7,"chacha20-ietf-poly1305"},
            };


            configTemplate = JObject.Parse(resData("config_tpl"));
            configDefault = JObject.Parse(resData("config_def"));
            configEditing = LoadServerConfig();

            cboxServList.Items.Clear();
            for (int i = 0; i < settings.GetServeNum(); i++)
            {
                cboxServList.Items.Add(i + 1);
            }

            perServIndex = settings.curEditingIndex;
            cboxServList.SelectedIndex = perServIndex;

            perConfigComponetIndex = 0;
            cboxConfigPart.SelectedIndex = perConfigComponetIndex;
            ShowConfigByIndex(perConfigComponetIndex);
        }
    }
}
