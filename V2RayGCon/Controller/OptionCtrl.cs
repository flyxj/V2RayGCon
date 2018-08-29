namespace V2RayGCon.Controller
{
    class OptionCtrl : Model.BaseClass.FormController
    {
        public bool IsOptionsSaved()
        {
            foreach (var component in GetAllComponents())
            {
                if ((component.Value as Controller.OptionComponent.OptionComponentController)
                    .IsOptionsChanged())
                {
                    return false;
                }
            }

            return true;
        }

        public bool SaveAllOptions()
        {
            var changed = false;

            foreach (var kvPair in GetAllComponents())
            {
                var component = kvPair.Value as Controller.OptionComponent.OptionComponentController;

                if (component.SaveOptions())
                {
                    changed = true;
                }
            }

            return changed;
        }

    }
}
