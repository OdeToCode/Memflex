using System;
using System.Collections.Generic;
using FlexProviders.Membership;
using FlexProviders.Raven;
using FlexProviders.Roles;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Listeners;

namespace FlexProviders.Tests.Integration.Raven
{
    public class IntegrationTest : IDisposable
    {
        protected readonly FlexMembershipProvider MembershipProvider;
        protected readonly FlexRoleProvider RoleProvider;
        protected readonly FakeApplicationEnvironment Environment;
        protected FlexMembershipUserStore<User, Role> UserStore;
        protected EmbeddableDocumentStore DocumentStore;
        protected IDocumentSession Session;
        public IDocumentSession Verifier
        {
            get 
            { 
                var session = DocumentStore.OpenSession();
                _resources.Add(session);
                return session;
            }
        }


        public IntegrationTest()
        {
            DocumentStore = new EmbeddableDocumentStore()
            {
                RunInMemory = true 
            };
            DocumentStore.RegisterListener(new NoStaleQueries());
            DocumentStore.Initialize();
            Session = DocumentStore.OpenSession();
            UserStore = new FlexMembershipUserStore<User, Role>(Session);
            Environment = new FakeApplicationEnvironment();
            RoleProvider = new FlexRoleProvider(UserStore);
            MembershipProvider = new FlexMembershipProvider(UserStore, Environment);
        }

        public void Dispose()
        {
            Session.Dispose();
            DocumentStore.Dispose();
            foreach (var disposable in _resources)
            {
                disposable.Dispose();
            }
        }

        class NoStaleQueries : IDocumentQueryListener
        {
            public void BeforeQueryExecuted(IDocumentQueryCustomization queryCustomization)
            {
                queryCustomization.WaitForNonStaleResults();
            }
        }

        private List<IDisposable> _resources = new List<IDisposable>();
    }
}