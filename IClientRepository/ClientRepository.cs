using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using IClientRepository.Domain;
using System.ServiceModel;
using Contracts;

namespace IClientRepository
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ClientRepository : Contracts.IClientRepository
    {
        public Guid CreateClient(string name, string lastname, string pesel, string adres)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    ClientRepo2 newClient = new ClientRepo2 { Name = name, Lastname=lastname, PESEL=pesel, address=adres};
                    session.Save(newClient);
                    transaction.Commit();
                }
                var result = session.QueryOver<ClientRepo2>().Where(x => x.PESEL == pesel).SingleOrDefault();
                return result.IdClient;
            }
        }

        public ClientRepo GetClientInformationByName(string name, string lastName)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.QueryOver<ClientRepo>().Where( x => x.Name == name && x.Lastname == lastName).SingleOrDefault();
                return result?? new ClientRepo();
            }
        }

        public ClientRepo GetClientInformationById(Guid Id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var result = session.QueryOver<ClientRepo>().Where(x => x.IdClient == Id).SingleOrDefault();
                return result ?? new ClientRepo();
            }
        }

        public void RemoveClientByName(string name, string lastname)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(session.QueryOver<ClientRepo2>().Where( x => x.Name == name && x.Lastname == lastname).SingleOrDefault());
                    transaction.Commit();
                }
            }
        }

        public void RemoveClientById(Guid Id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(Id); 
                    transaction.Commit();
                }
            }
        }
    }
}
