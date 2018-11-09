namespace V2RayGCon.Model.BaseClass
{
    public class FormComponentController : IFormComponentController
    {
        protected FormController container;

        // bind UI controls with component
        public void Bind(FormController container)
        {
            this.container = container;
        }

        public T GetContainer<T>() where T : FormController
        {
            return this.container as T;
        }
    }
}
