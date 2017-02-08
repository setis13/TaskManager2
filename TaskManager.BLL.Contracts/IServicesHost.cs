using TaskManager.BLL.Contracts.Services.Base;

namespace TaskManager.BLL.Contracts {
    /// <summary>
    ///     Service Host contract </summary>
    public interface IServicesHost {
        /// <summary>
        ///     Register Service with type T </summary>
        /// <typeparam name="T">Service Type</typeparam>
        /// <param name="service">Service instance</param>
        void Register<T>(T service) where T : IService;

        /// <summary>
        ///     Get Service by it's type </summary>
        /// <typeparam name="T">Service type</typeparam>
        /// <returns>Service instance</returns>
        T GetService<T>() where T : IService;
    }
}