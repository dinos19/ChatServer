namespace ChatServer.Infrastructure.Repositories.BaseAbstractions
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private ApiDbContext dbContext;
        private AccountRepository _account;

        public AccountRepository Account
        {
            get
            {
                if (_account == null)
                {
                    _account = new AccountRepository(dbContext);
                }
                return _account;
            }
        }

        private ChatMessageRepository _chatMessage;

        public ChatMessageRepository ChatMessage
        {
            get
            {
                if (_chatMessage == null)
                {
                    _chatMessage = new ChatMessageRepository(dbContext);
                    return _chatMessage;
                }

                return _chatMessage;
            }
        }

        private UserConnectionRepository _userConnection;

        public UserConnectionRepository UserConnection
        {
            get
            {
                if (_userConnection == null)
                {
                    _userConnection = new UserConnectionRepository(dbContext);
                    return _userConnection;
                }

                return _userConnection;
            }
        }

        private UploadsRepository _uploads;

        public UploadsRepository Uploads
        {
            get
            {
                if (_uploads == null)
                {
                    _uploads = new UploadsRepository(dbContext);
                }
                return _uploads;
            }
        }

        public RepositoryWrapper(ApiDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }
    }
}