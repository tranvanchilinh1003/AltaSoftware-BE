using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

public class TestsAttachmentMapper : Profile
{
    public TestsAttachmentMapper()
    {
        CreateMap<TestsAttachmentRequest, TestsAttachment>();
        CreateMap<TestsAttachment, TestsAttachmentResponse>();
    }
}
