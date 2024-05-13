using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BanHangThoiTrangMVC.Models.Common
{
    public class ThongKeTruyCap
    {
        private readonly IConfiguration _configuration;

        public ThongKeTruyCap(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ThongKeViewModel ThongKe()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connect = new SqlConnection(connectionString))
            {
                var item = connect.QueryFirstOrDefault<ThongKeViewModel>("sp_Thongke", commandType: CommandType.StoredProcedure); //sp_Thongke là do proc ở SQL viết từ query ra
                return null;
            }
        }
    }
}