using V2RayGCon.Model.BaseClass;

namespace V2RayGCon.Controller.OptionComponent
{
    class OptionComponentController : IFormComponentController
    {
        private FormComponentController auxComponentController
            = new FormComponentController();

        public void Bind(Model.BaseClass.FormController container)
        {
            auxComponentController.Bind(container);
        }

        protected OptionCtrl GetContainer()
        {
            return auxComponentController.GetContainer<OptionCtrl>();
        }
    }
}
