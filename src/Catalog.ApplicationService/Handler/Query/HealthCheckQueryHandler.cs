using Catalog.ApiContract.Request.Query;
using Catalog.ApplicationService.Communicator.User;
using Catalog.Domain;
using Framework.Core.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.ApplicationService.Handler.Query
{
    public class HealthCheckQueryHandler : IRequestHandler<HealthCheckQuery, ResponseBase<object>>
    {
        private readonly IDbContextHandler _dbContextHandler;
        private readonly IUserCommunicator _userCommunicator;
        public HealthCheckQueryHandler(IDbContextHandler dbContextHandler, IUserCommunicator userCommunicator)
        {
            _dbContextHandler = dbContextHandler;
            _userCommunicator = userCommunicator;
        }

        public Task<ResponseBase<object>> Handle(HealthCheckQuery request, CancellationToken cancellationToken)
        {
            //1.yontem
            var hcheck = new HealtCheck(_userCommunicator);

            var result = _userCommunicator.IsUp();

            //2. yontem
            hcheck = new HealtCheck(result);


            //throw new BusinessRuleException(5,"test","asd");
            return Task.FromResult(new ResponseBase<object>
            {
                Success = true,
                Message = "CategoryApi I'm alive and well. Don't bother me while counting cashes!",
                MessageCode = ApplicationMessage.Success
            });
        }
    }

    public class HealtCheck
    {
        public bool IsUserServiceUp { get; protected set; }
        public HealtCheck(IUserCommunicator userCommunicator)
        {
            IsUserServiceUp = userCommunicator.IsUp();
        }

        public HealtCheck(bool isUserServiceUp)
        {
            IsUserServiceUp = isUserServiceUp;
        }
    }
}