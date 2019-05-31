using Topshelf;

namespace CertsureMonitoringServices
{
    class Program
    {
        static void Main(string[] args)
        {

            HostFactory.Run(serviceConfig =>
            {
                serviceConfig.Service<LoginService>(serviceInstance =>
                                                    {
                                                        serviceInstance.ConstructUsing(
                                                            () => new LoginService());
                                                        serviceInstance.WhenStarted(execute => execute.Start());
                                                        serviceInstance.WhenStopped(execute => execute.Stop());

                                                    });
                serviceConfig.SetServiceName("NOCSLoginMonitoring");
                serviceConfig.SetDisplayName("NOCS Login Monitoring Service");
                serviceConfig.SetDescription("Monitor and sent Email alert when login is not possible in NOCS Application");
                serviceConfig.StartAutomatically();

            });
        } 
    }
}
