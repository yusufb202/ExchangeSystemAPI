using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService
    {
        private readonly IRepository<UserActivityReport> _userActivityReportRepository;

        public UserService(IRepository<UserActivityReport> userActivityReportRepository)
        {
            _userActivityReportRepository = userActivityReportRepository;
        }

        public async Task<IEnumerable<UserActivityReport>> GetUserActivityReportsAsync()
        {
            return await _userActivityReportRepository.GetAllAsync(); // Assuming GetAllAsync is defined
        }
    }

}
