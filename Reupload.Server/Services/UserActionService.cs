using System.Net;
using Reupload.Server.Data.Repositories.Contracts;
using Reupload.Server.Dtos.UserActions;
using Reupload.Server.Enums;
using Reupload.Server.Exceptions;
using Reupload.Server.Models;
using Reupload.Server.Services.Contracts;
using Reupload.Shared.Pagination;

namespace Reupload.Server.Services;

public class UserActionService : IUserActionService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserActionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task InsertAsync(UserActionInsertDto userActionInsertDto)
    {
        UserAction userAction = UserAction.FromUserActionInsertDto(userActionInsertDto);

        await _unitOfWork.UserActions.AddAsync(userAction);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PagedList<UserActionTableDto>> GetPaginatedTableAsync(PaginationRequestDto paginationRequestDto)
    {
        PagedList<UserActionTableDto> paginatedUserActions = await _unitOfWork.UserActions.PaginatedDetailsAsync(paginationRequestDto);

        return paginatedUserActions;
    }

    public async Task<UserActionDetailsDto> GetDetailsAsync(Guid userActionId)
    {
        UserAction? userAction = await _unitOfWork.UserActions.FirstDetailsAsync(x => x.Id == userActionId);

        if (userAction is null)
        {
            throw new UserActionException(
                ErrorCode.UserActionNotFound, 
                HttpStatusCode.NotFound,
                $"User action with ID {userActionId} not found.");
        }

        return UserActionDetailsDto.FromUserAction(userAction);
    }
}