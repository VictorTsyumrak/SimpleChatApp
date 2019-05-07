namespace SimpleChatApp.Domain.Options
{
    public class ChatOptions
    {
        public RoomOptions Room { get; set; } = new RoomOptions();
        public int NumberOfRooms { get; set; } = 3;
    }
}