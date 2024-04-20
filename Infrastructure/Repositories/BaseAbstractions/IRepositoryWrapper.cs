namespace ChatServer.Infrastructure.Repositories.BaseAbstractions
{
    public interface IRepositoryWrapper
    {
        AccountRepository Account { get; }
        ChatMessageRepository ChatMessage { get; }
        UserConnectionRepository UserConnection { get; }
        UploadsRepository Uploads { get; }

        void Save();
    }
}