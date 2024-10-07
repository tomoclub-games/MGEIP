using System.Collections;
using System.Collections.Generic;
using Realms;
using MongoDB.Bson;
using MGIEP.Data;

public class GameDataModel : RealmObject
{
    [PrimaryKey]
    public string Id { get; set; }

    public int score { get; set; }

    public MGIEPData mgiepData { get; set; }
}
