using System.Dynamic;

namespace SSISTests.TestInfrastructure
{
    public class AssertTable : DynamicObject
    {
        private readonly string _connectionString;

        public AssertTable(string connectionString)
        {
            _connectionString = connectionString;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = new DatabaseRowAssertion(_connectionString, binder.Name);

            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = new DatabaseRowAssertion(_connectionString, binder.Name);

            return true;
        }
    }
}
