using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace ShowDetailViewFromDashboard.Module.BusinessObjects {
    [DefaultClassOptions]
    public class Contact : Person {
        public Contact(Session session) : base(session) {
        }
    }
}