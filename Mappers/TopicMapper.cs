using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TopicRequest, Topic>(); // Mapping từ DTO -> Entity
        CreateMap<Topic, TopicResponse>(); // Mapping từ Entity -> DTO (nếu cần)
                                           // Mapping từ DTO Request -> Model
        CreateMap<TopicsFileRequest, TopicsFile>();
        // Mapping từ Model -> DTO Response
        CreateMap<TopicsFile, TopicsFileResponse>();
    }   
}
