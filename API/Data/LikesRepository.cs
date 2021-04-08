using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;
        public LikesRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<UserLike> GetUserLike(int sourceUseId, int likedUserId)
        {
            return await _context.Likes.FindAsync(sourceUseId, likedUserId);
        }

        public async Task<PageList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();

            if(likesParams.Predicate == "liked")
            {
                likes = likes.Where(likes => likes.SourceUserId == likesParams.UserId);
                users = likes.Select(like => like.LikedUser);
            }

            if (likesParams.Predicate == "likedBy")
            {
               likes = likes.Where(likes => likes.LikedUserId == likesParams.UserId);
                users = likes.Select(like => like.SourceUser); 
            }

            var Likeusers = users.Select(user => new LikeDto
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                City = user.City,
                Id = user.Id
            });

            return await PageList<LikeDto>.CreateAsync(Likeusers, likesParams.PageNumber, likesParams.PageSize );




        }

        public async Task<AppUser> GetUseWithLikes(int userId)
        {
            return await _context.Users
              .Include(x => x.LikedUsers)
              .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}