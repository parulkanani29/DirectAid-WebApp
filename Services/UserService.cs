using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Dapper;
using SharedModels;

namespace Services
{
    
    public class UserService : IUserService
    {
        private readonly IDatabaseSettings _databaseSettings;
        public UserService(IDatabaseSettings databaseSettings)
        {
            _databaseSettings = databaseSettings;
        }

        public async Task<Guid> Register(User user)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(_databaseSettings.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    DynamicParameters parameter = new DynamicParameters();
                    parameter.Add("@WalletName", user.WalletName);
                    parameter.Add("@WalletAddress", user.WalletAddress);
                    parameter.Add("@UserRole", user.Role);
                    parameter.Add("@FirstName", user.FirstName);
                    parameter.Add("@LastName", user.LastName);
                    parameter.Add("@Email", user.Email);
                    parameter.Add("@BirthDate", user.BirthDate);
                    var result = await con.QueryFirstAsync<Guid>("RegisterUser", parameter, commandType: CommandType.StoredProcedure, commandTimeout: 600);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<User> GetUserByWallet(string walletName)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(_databaseSettings.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    DynamicParameters parameter = new DynamicParameters();
                    parameter.Add("@WalletName", walletName);                    
                    var result = await con.QueryFirstOrDefaultAsync<User>("GetUserByWallet", parameter, commandType: CommandType.StoredProcedure, commandTimeout: 600);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateApplicationStatus(string id, int status)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(_databaseSettings.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    DynamicParameters parameter = new DynamicParameters();
                    parameter.Add("@ApplicationId", id);
                    parameter.Add("@Status", status);

                    return  await con.QueryFirstAsync<int>("UpdateApplicationStatus", parameter, commandType: CommandType.StoredProcedure, commandTimeout: 600);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Guid> CreateApplication(Application application)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(_databaseSettings.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    DynamicParameters parameter = new DynamicParameters();
                    parameter.Add("@Category", application.Category);
                    parameter.Add("@Applicant", application.UserId);
                    parameter.Add("@IdentificationNumber", application.IdentificationNumber);
                    parameter.Add("@Status", application.Status);
                    parameter.Add("@AverageIncome", application.AverageIncome);

                    return await con.QueryFirstAsync<Guid>("CreateApplication", parameter, commandType: CommandType.StoredProcedure, commandTimeout: 600);                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Application>> GetAllApplications()
        {
            try
            {
                using (IDbConnection con = new SqlConnection(_databaseSettings.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    DynamicParameters parameter = new DynamicParameters();                    

                    var result = await con.QueryAsync<Application>("GetAllApplications", parameter, commandType: CommandType.StoredProcedure, commandTimeout: 600);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Application>> GetApplicationsByUser (string userId)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(_databaseSettings.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    DynamicParameters parameter = new DynamicParameters();
                    parameter.Add("@UserId", userId);

                    var result = await con.QueryAsync<Application>("GetApplicationsByUser", parameter, commandType: CommandType.StoredProcedure, commandTimeout: 600);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
