using EventAPI.Core.Data.DTO;
using EventAPI.Core.Data.Entities;

namespace EventAPI.Core.Interfaces
{
    public interface IAyrshare
    {
        public Task<string> PostYourEvent(string postDescription);

    }
}
