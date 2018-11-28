using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VgcApis.Models
{
    public interface IServersService
    {
        event EventHandler<BoolEvent> OnServerStateChange;
        event EventHandler<StrEvent> OnCoreClosing;

        string PackServersIntoV4Package(
            List<ICoreCtrl> servList,
            string orgServerUid,
            string packageName);

        ReadOnlyCollection<ICoreCtrl> GetTrackableServerList();
        ReadOnlyCollection<ICoreCtrl> GetAllServersList();
    }
}
