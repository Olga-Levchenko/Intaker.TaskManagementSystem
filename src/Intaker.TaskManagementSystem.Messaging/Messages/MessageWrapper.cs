namespace Intaker.TaskManagementSystem.Messaging.Messages
{
    internal class MessageWrapper
    {
        public required string Type { get; set; }
        public required string Message { get; set; }
    }
}
