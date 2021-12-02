using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using ShowDetailViewFromDashboard.Module.BusinessObjects;
using System.Reflection;
using DevExpress.ExpressApp.Dashboards;

namespace ShowDetailViewFromDashboard.Module.DatabaseUpdate {
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            Contact contactMary = ObjectSpace.FindObject<Contact>(CriteriaOperator.Parse("FirstName == 'Mary' && LastName == 'Tellitson'"));
            if(contactMary == null) {
                contactMary = ObjectSpace.CreateObject<Contact>();
                contactMary.FirstName = "Mary";
                contactMary.LastName = "Tellitson";
                contactMary.Email = "mary_tellitson@md.com";
                contactMary.Birthday = new DateTime(1980, 11, 27);
            }
            Contact contactJohn = ObjectSpace.FindObject<Contact>(CriteriaOperator.Parse("FirstName == 'John' && LastName == 'Nilsen'"));
            if(contactJohn == null) {
                contactJohn = ObjectSpace.CreateObject<Contact>();
                contactJohn.FirstName = "John";
                contactJohn.LastName = "Nilsen";
                contactJohn.Email = "john_nilsen@md.com";
                contactJohn.Birthday = new DateTime(1981, 10, 3);
            }
            Contact contactAndrew = ObjectSpace.FindObject<Contact>(CriteriaOperator.Parse("FirstName == 'Andrew' && LastName == 'Fuller'"));
            if(contactAndrew == null) {
                contactAndrew = ObjectSpace.CreateObject<Contact>();
                contactAndrew.FirstName = "Andrew";
                contactAndrew.LastName = "Fuller";
                contactAndrew.Email = "andrewfuller@example.com";
                contactAndrew.Birthday = new DateTime(1952, 3, 19);
            }
            Contact contactRobert = ObjectSpace.FindObject<Contact>(CriteriaOperator.Parse("FirstName == 'Robert' && LastName == 'King'"));
            if(contactRobert == null) {
                contactRobert = ObjectSpace.CreateObject<Contact>();
                contactRobert.FirstName = "Robert";
                contactRobert.LastName = "King";
                contactRobert.Email = "robertking@example.com";
                contactRobert.Birthday = new DateTime(1975, 4, 25);
            }
            Assembly assembly = Assembly.GetExecutingAssembly();
            DashboardsModule.AddDashboardDataFromResources<DashboardData>(ObjectSpace, "Contacts", assembly, "ShowDetailViewFromDashboard.Module.Dashboards.Contacts.xml");

            ObjectSpace.CommitChanges();
        }
    }
}
