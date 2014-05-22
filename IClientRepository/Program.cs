using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg;
using IClientRepository.Domain;
using NHibernate.Tool.hbm2ddl;
using log4net;
using log4net.Repository.Hierarchy;
using System.ServiceModel;
using Contracts;
using System.ServiceModel.Description;
using System.Timers;

namespace IClientRepository
{
    class Program
    {
        public const string serviceAddress = "net.tcp://localhost:54390/IClientRepository";
        public const string serviceRepositoryAddress = "net.tcp://localhost:11900/IServiceRepository";
        public const string serviceName = "IClientRepository";
       // public const string ipAddress = "localhost";
        public static IServiceRepository repository { get; set; }
        static void Main(string[] args)
        {
              //Logger
            log4net.Config.XmlConfigurator.Configure();

            //Configuration
            ClientRepository accountRep = new ClientRepository();
            ServiceHost sh = new ServiceHost(accountRep, new Uri[] { new Uri(serviceAddress) });
            sh.AddServiceEndpoint(typeof(Contracts.IClientRepository), new NetTcpBinding(SecurityMode.None), serviceAddress);
           
            //Service starting
            sh.Open();
            Logger.Info("IClientRepository started!");


            //Register service in IServiceRepository
            ChannelFactory<IServiceRepository> cf = new ChannelFactory<IServiceRepository>(new NetTcpBinding(SecurityMode.None), serviceRepositoryAddress);
            repository = cf.CreateChannel();
            Logger.Info("Connection with IServiceRepository completed!");
            var timer = new System.Threading.Timer(e => imAlive(repository), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
          
            repository.registerService(serviceName, serviceAddress.Replace("localhost", serviceName));
            Logger.Info("Service registered!");


            //Send info for IServiceRepository
            AliveSignal();
            Logger.Info("Alive");


            //Sending information that service is alive every 10 minutes
            Timer t = new Timer(600000);
            t.AutoReset = true;
            t.Elapsed += new System.Timers.ElapsedEventHandler(Alive);
            t.Start();


            //Click to close service
            Console.ReadLine();


            //Unregisted service from IServiceRepository
            repository.unregisterService(serviceName);

            //Service closing
            Logger.Info("IClientRepository closing!");


           /*
            ClientRepository repo = new ClientRepository();
            
            //CREATE!
           var MikeAbyss = new Client { Name = "Wojciech2", Lastname = "Zięba", PESEL = "9201081223", address = "Sandomierz" };
           Guid dupa =  repo.CreateClient(MikeAbyss);
           Console.WriteLine(dupa);
          /*  //read
            Client client = repo.GetClientInformationByName("MikeAbyss","700");


            //delete
            client.Name = "MikeAbyss";
            client.Lastname = "700";
            repo.RemoveClientByName(client);
            */
            Console.Read();
        }
        private static void Alive(object sender, System.Timers.ElapsedEventArgs e)
        {
            AliveSignal();
        }

        private static void imAlive(IServiceRepository serviceRepository)
        {
            serviceRepository.isAlive("net.tcp://localhost:11900/IServiceRepository");
            Console.WriteLine("Wysyłanie sygnału IamAlive");
        }

        private static void AliveSignal()
        {
                repository.isAlive(serviceName);
                Logger.Info("Alive Success");
        }
    }

    public static class Logger
    {
        public static void Info(string Message)
        {
            Console.WriteLine(Message);
        }
    }
}
