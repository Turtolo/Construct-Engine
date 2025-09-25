using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ConstructEngine.Util;


public class TiledReader
{

    public class TiledMap
    {
        public int compressionlevel { get; set; }
        public int height { get; set; }
        public bool infinite { get; set; }
        public List<TiledLayer> layers { get; set; }
        public int nextlayerid { get; set; }
        public int nextobjectid { get; set; }
        public string orientation { get; set; }
        public string renderorder { get; set; }
        public string tiledversion { get; set; }
        public int tileheight { get; set; }
        public List<Tileset> tilesets { get; set; }
        public int tilewidth { get; set; }
        public string type { get; set; }
        public string version { get; set; }
        public int width { get; set; }
    }

    public class TiledLayer
    {
        public List<int> data { get; set; } // For tile layers
        public int height { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public float opacity { get; set; }
        public string type { get; set; }
        public bool visible { get; set; }
        public int width { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public List<TiledObject> objects { get; set; } // For object layers
    }

    public class TiledObject
    {
        public string name { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public float width { get; set; }
        public float height { get; set; }
        public List<TiledProperty> properties { get; set; }
    }

    public class TiledProperty
    {
        public string name { get; set; }
        public string type { get; set; }
        public object value { get; set; }
    }

    public class Tileset
    {
        public int firstgid { get; set; }
        public string source { get; set; }
    }


}