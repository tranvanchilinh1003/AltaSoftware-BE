using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;

namespace ISC_ELIB_SERVER.Services
{
    public interface IChatService
    {
        ApiResponse<ICollection<ChatResponse>> GetChatsBySessionId(long sessionId);
        ApiResponse<ChatResponse> GetChatById(long id);
        ApiResponse<ChatResponse> CreateChat(ChatRequest chatRequest);
        ApiResponse<ChatResponse> UpdateChat(long id, ChatUpdateRequest chatRequest);
        ApiResponse<object> DeleteChat(long id);
    }

    public class ChatService : IChatService
    {
        private readonly ChatRepo _repository;
        private readonly IMapper _mapper;

        public ChatService(ChatRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //Lấy danh sách tin nhắn theo sessionId
        public ApiResponse<ICollection<ChatResponse>> GetChatsBySessionId(long sessionId)
        {
            var chats = _repository.GetChatsBySessionId(sessionId);
            var response = _mapper.Map<ICollection<ChatResponse>>(chats);

            return chats.Any()
                ? ApiResponse<ICollection<ChatResponse>>.Success(response)
                : ApiResponse<ICollection<ChatResponse>>.NotFound("Không có tin nhắn nào trong phiên này.");
        }

        //Lấy tin nhắn theo Id
        public ApiResponse<ChatResponse> GetChatById(long id)
        {
            var chat = _repository.GetChatById(id);
            return chat != null
                ? ApiResponse<ChatResponse>.Success(_mapper.Map<ChatResponse>(chat))
                : ApiResponse<ChatResponse>.NotFound($"Không tìm thấy tin nhắn #{id}");
        }

        //Tạo tin nhắn mới
        public ApiResponse<ChatResponse> CreateChat(ChatRequest chatRequest)
        {
            var chat = new Chat
            {
                Content = chatRequest.Content,
                UserId = chatRequest.UserId,
                SessionId = chatRequest.SessionId,
                SentAt = DateTime.UtcNow
            };

            var createdChat = _repository.CreateChat(chat);
            return ApiResponse<ChatResponse>.Success(_mapper.Map<ChatResponse>(createdChat));
        }

        //Sửa tin nhắn 
        public ApiResponse<ChatResponse> UpdateChat(long id, ChatUpdateRequest chatRequest)
        {
            var chat = _repository.GetChatById(id);
            if (chat == null)
            {
                return ApiResponse<ChatResponse>.NotFound($"Không tìm thấy tin nhắn #{id}");
            }

            chat.Content = chatRequest.Content;
            chat.SentAt = DateTime.UtcNow;

            var updatedChat = _repository.UpdateChat(chat);
            return ApiResponse<ChatResponse>.Success(_mapper.Map<ChatResponse>(updatedChat));
        }

        // Xóa tin nhắn
        public ApiResponse<object> DeleteChat(long id)
        {
            var success = _repository.DeleteChat(id);
            return success
                ? ApiResponse<object>.Success("Tin nhắn đã được xóa.")
                : ApiResponse<object>.NotFound("Không tìm thấy tin nhắn để xóa.");
        }
    }
}
