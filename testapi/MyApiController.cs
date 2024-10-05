using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using testapi;

namespace MyApp.Namespace
{

    [Route("api/[controller]")]
    [ApiController]
    public class MyApiController : ControllerBase
    {
        [Route("baitap210")]
        [HttpPost]
        public int baitap210([FromForm] int[] request)
        {
            return request
            .Where(num =>
            {
                var firstDigit = Math.Abs(num).ToString()[0];
                return (firstDigit - '0') % 2 == 0;
            }).Sum();
        }


        [Route("baitap211")]
        [HttpPost]
        public double baitap211([FromBody] int[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
            {
                return 0;
            }

            var result = TinhTrungBinhSoNguyenTo(numbers);
            return result;
        }


        [Route("baitap212")]
        [HttpPost]
        public double baitap212([FromBody] int[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
            {
                return 0;
            }
            var positiveNumbers = numbers.Where(num => num > 0).ToList();

            if (!positiveNumbers.Any()) return 0;

            return positiveNumbers.Average();
        }

        [Route("baitap213")]
        [HttpPost]
        public double baitap213([FromBody] int[] numbers, int x)
        {
            if (numbers == null || numbers.Length == 0)
            {
                return 0;
            }

            var greaterThanX = numbers.Where(num => num > x).ToList();

            if (!greaterThanX.Any()) return 0;


            return greaterThanX.Average();
        }


        [Route("baitap214")]
        [HttpPost]
        public double baitap214([FromBody] int[] numbers)
        {

            var positiveNumbers = numbers.Where(num => num > 0).ToList();

            if (!positiveNumbers.Any()) return 0;

            double product = positiveNumbers.Aggregate(1.0, (acc, num) => acc * num);

            return Math.Pow(product, 1.0 / positiveNumbers.Count);

        }


        public static bool IsPrime(int number)
        {
            if (number < 2) return false;
            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0) return false;
            }
            return true;
        }

        public static double TinhTrungBinhSoNguyenTo(int[] numbers)
        {
            var primeNumbers = numbers.Where(num => IsPrime(num)).ToList();

            if (!primeNumbers.Any()) return 0;

            return primeNumbers.Average();
        }



        [Route("getPlace")]
        [HttpGet]
        public List<AdsPlaceEntity> getPlace()
        {


            using (var db = new AdsMongoDbContext())
            {

                var placesData = db.AdsPlace.ToList();

                return placesData;
            }


        }

        [Route("insertPlace")]
        [HttpPost]
        public async Task<ApiResponse<InsertPlaceResponse>> inserPlace([FromBody] CreatePlaceRequest request)
        {

            using (var db = new AdsMongoDbContext())
            {
                var existed = db.AdsPlace.Where(i => i.PlaceName == request.PlaceName).FirstOrDefault();
                if (existed == null)
                {
                    AdsPlaceEntity objs = new AdsPlaceEntity
                    {
                        PlaceName = request.PlaceName,
                        PlaceAddress = request.PlaceAddress,
                    };


                    await db.AdsPlace.Insert(objs);
                    return new ApiResponse<InsertPlaceResponse> {
                        Status = 1,
                        Data = {PlaceName = request.PlaceName, PlaceAddress = request.PlaceAddress}
                    };
                }
                 return new ApiResponse<InsertPlaceResponse>{
                    Status = 0,
                 }; ;
            }
        }

        [Route("updatePlace")]
        [HttpPatch]
        public async Task<ApiResponse<UpdatePlaceResponse>> updatePlace([FromBody] UpdatePlaceRequest request)
        {

            using (var db = new AdsMongoDbContext())
            {
                var placeObjectId = ObjectId.Parse(request.PlaceId);
                var existed = db.AdsPlace.Where(i => i.Id == placeObjectId).FirstOrDefault();
                if (existed != null)
                {

                    existed.PlaceName = request.PlaceName;
                    existed.PlaceAddress = request.PlaceAddress;
                    await db.AdsPlace.Update(existed);
                    return new ApiResponse<UpdatePlaceResponse>
                    {
                        Status = 1,
                        Data = new UpdatePlaceResponse
                        {
                            Id = placeObjectId
                        }
                    };
                }
                return new ApiResponse<UpdatePlaceResponse>
                {
                    Status = 0
                };
            }
        }


        [Route("deletePlace")]
        [HttpDelete]
        public async Task<bool> deletePlace([FromBody] string placeId)
        {

            using (var db = new AdsMongoDbContext())
            {
                var placeObjectId = ObjectId.Parse(placeId);
                var existed = db.AdsPlace.Where(i => i.Id == placeObjectId).FirstOrDefault();
                if (existed != null)
                {

                    await db.AdsPlace.Delete(placeObjectId);
                    return true;
                }
                return false;
            }
        }
    }







}

public class CreatePlaceRequest
{
    public string PlaceName { get; set; }
    public string PlaceAddress { get; set; }
}


public class UpdatePlaceRequest
{

    public string PlaceId { get; set; }
    public string PlaceName { get; set; }
    public string PlaceAddress { get; set; }
}

public class ApiResponse<T>
{
    public int Status { get; set; }
    public T? Data { get; set; }
}

public class UpdatePlaceResponse
{
    public ObjectId Id { get; set; }

}


public class InsertPlaceResponse
{
    public string PlaceName { get; set; }

    public string PlaceAddress { get; set; }

}