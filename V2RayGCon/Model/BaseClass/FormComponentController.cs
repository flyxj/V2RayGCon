namespace V2RayGCon.Model.BaseClass
{
    class FormComponentController : IFormComponentController
    {
        protected FormController container;

        // bind UI controls with component
        public void Bind(FormController container)
        {
            this.container = container;
        }
    }
}
