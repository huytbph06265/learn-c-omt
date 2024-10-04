using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using testapi;

namespace MyApp.Namespace
{

    //using (var db = new AdsMongoDbContext()){
    //    var placesData = db.AdsPlace.Where(i => i.UserOwnerId == MongoDB.Bson.ObjectId.Parse(value.userOwnerId)).ToList();
    //}


    //using (var db = new AdsMongoDbContext()){
    //    var existed = db.AdsPlace.Where(i => i.PlaceName == placeName && i.UserOwnerId == userOwnerId).FirstOrDefault();
    //    if (existed == null){
    //        AdsPlaceEntity objs = new AdsPlaceEntity
    //        {
    //            PlaceName = placeName,
    //            UserOwnerId = userOwnerId,
    //            PlaceAddress = placeAddress,
    //        };
    //        if (placeId != null)
    //        {
    //            objs.Id = placeId.Value;
    //        }

    //        await db.AdsPlace.Insert(objs);
    //    }
    //}

    //using (var db = new AdsMongoDbContext())
    //{
    //    await db.AdsPlace.UpdatePartial(i => i.Id == placeId,
    //    new Dictionary<System.Linq.Expressions.Expression<Func<AdsPlaceEntity, object>>, dynamic> { { f => f.UserOwnerId, userOwnerId }, }
    //    );
    //}

    //using (var db = new AdsMongoDbContext()){
    //    await db.AdsPlace.Delete(placeId);
    //}


    [Route("api/[controller]")]
    [ApiController]
    public class MyApiController : ControllerBase
    {
        [Route("baitap210")]
        [HttpPost]
        public async Task<int> baitap210([FromForm] int[] request)
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
        public async Task<double> baitap211([FromBody] int[] numbers)
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
        public async Task<double> baitap212([FromBody] int[] numbers)
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
        public async Task<double> baitap213([FromBody] int[] numbers, int x)
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
        public async Task<double> baitap214([FromBody] int[] numbers)
        {

            var positiveNumbers = numbers.Where(num => num > 0).ToList(); // Lọc các số dương

            if (!positiveNumbers.Any()) return 0; // Nếu không có số dương, trả về 0

            double product = positiveNumbers.Aggregate(1.0, (acc, num) => acc * num); // Tích các số dương

            return Math.Pow(product, 1.0 / positiveNumbers.Count);

        }


        public static bool IsPrime(int number)
        {
            if (number < 2) return false; // Số nguyên tố nhỏ nhất là 2
            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0) return false;
            }
            return true;
        }

        // Hàm tính trung bình cộng các số nguyên tố trong mảng
        public static double TinhTrungBinhSoNguyenTo(int[] numbers)
        {
            var primeNumbers = numbers.Where(num => IsPrime(num)).ToList();

            if (!primeNumbers.Any()) return 0; // Nếu không có số nguyên tố nào, trả về 0

            return primeNumbers.Average(); // Tính trung bình cộng các số nguyên tố
        }


        [Route("add")]
        [HttpPost]
        public async Task<double> add([FromBody] int[] numbers)
        {

            var positiveNumbers = numbers.Where(num => num > 0).ToList(); // Lọc các số dương

            if (!positiveNumbers.Any()) return 0; // Nếu không có số dương, trả về 0

            double product = positiveNumbers.Aggregate(1.0, (acc, num) => acc * num); // Tích các số dương

            return Math.Pow(product, 1.0 / positiveNumbers.Count);

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
        public async Task<bool> inserPlace([FromBody] CreatePlaceRequest request)
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
                   return true;
                }
                return false;
            }
        }

        [Route("insertPlace")]
        [HttpPatch]
        public async Task<bool> updatePlace([FromBody] UpdatePlaceRequest request)
        {

            using (var db = new AdsMongoDbContext())
            {
                var placeObjectId = ObjectId.Parse(request.PlaceId);
                var existed = db.AdsPlace.Where(i => i.Id == placeObjectId).FirstOrDefault();
                if (existed == null)
                {

                    existed.PlaceName = request.PlaceName;
                    existed.PlaceAddress = request.PlaceAddress;
                    await db.AdsPlace.Update(existed);
                    return true;
                }
                return false;
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
                if (existed == null)
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