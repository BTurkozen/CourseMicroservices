using Course.Services.Discount.Dtos;
using Course.Shared.Dtos;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Discount.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        // Herhangi bir db ile iletişime geçmek için kullanılan interface.
        private readonly IDbConnection _dbConnection;

        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<Response<NoContent>> DeleteAsync(int id)
        {
            var status = await _dbConnection.ExecuteAsync("DELETE FROM Discount where id=@Id", new { id });

            if (status > 0)
            {
                return Response<NoContent>.Success(204);
            }

            return Response<NoContent>.Fail("Discount Not Found", 404);
        }

        public async Task<Response<List<DiscountDto>>> GetAllAsync()
        {
            var discountsDto = await _dbConnection.QueryAsync<DiscountDto>("SELECT * FROM Discount");

            return Response<List<DiscountDto>>.Success(discountsDto.ToList(), 200);
        }

        public async Task<Response<DiscountDto>> GetByCodeAndUserId(string code, string userId)
        {
            var discountsDto = await _dbConnection.QueryAsync<DiscountDto>("SELECT * FROM Discount WHERE userId=@UserId AND code=@Code", new { userId, code });

            var discountDto = discountsDto.FirstOrDefault();

            if (discountDto is null)
            {
                return Response<DiscountDto>.Fail("Discount not found", 404);
            }

            return Response<DiscountDto>.Success(discountDto, 200);
        }

        public async Task<Response<DiscountDto>> GetByIdAsync(int id)
        {
            var discountsDto = await _dbConnection.QueryAsync<DiscountDto>("SELECT * FROM Discount where id=@Id", new { id });

            var discountDto = discountsDto.FirstOrDefault();

            if (discountDto is null)
            {
                return Response<DiscountDto>.Fail("Discount not found", 404);
            }

            return Response<DiscountDto>.Success(discountDto, 200);
        }

        public async Task<Response<NoContent>> SaveAsync(DiscountCreateDto discountCreateDto)
        {
            var status = await _dbConnection.ExecuteAsync("INSERT INTO Discount(userId,rate,code) VALUES(@UserId,@Rate,@Code)", new { discountCreateDto.UserId, discountCreateDto.Rate, discountCreateDto.Code });

            if (status > 0)
            {
                return Response<NoContent>.Success(204);
            }

            return Response<NoContent>.Fail("An Error occurred while adding", 500);
        }

        public async Task<Response<NoContent>> UpdateAsync(DiscountUpdateDto discountUpdateDto)
        {
            var status = await _dbConnection.ExecuteAsync("UPDATE Discount SET userId=@UserId, code=@Code,rate=@Rate WHERE Id=@id", discountUpdateDto);

            if (status > 0)
            {
                return Response<NoContent>.Success(204);
            }
            return Response<NoContent>.Fail("Discount Not Found", 404);
        }
    }
}
