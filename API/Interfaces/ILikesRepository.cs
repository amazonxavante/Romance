using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUseId, int likedUserId);
        Task<AppUser> GetUseWithLikes(int userId);
        Task<PageList<LikeDto>> GetUserLikes(LikesParams likesParams);
    }
}