using cloud.core;
using cloud.core.mongodb;

namespace testapi
{
   
    public class AdsMongoDbContext : BaseMongoObjectIdDbContext
    {
        public AdsMongoDbContext() : base(AppSettingsHelper.GetValueByKey("AdsMongoDbContext:ConnectionString"))
        {

        }

        /// tương đương 1 collection
        public DbSetObjectId<AdsPlaceEntity> AdsPlace { get; set; }

    }


    public class AdsPlaceEntity : AbstractEntityObjectIdTracking
    {
        public string PlaceName { get; set; }
        public string PlaceAddress { get; set; }
        public int PlaceTotal { get; set; }

        public long LastUpdated { get; set; }
    }

}
