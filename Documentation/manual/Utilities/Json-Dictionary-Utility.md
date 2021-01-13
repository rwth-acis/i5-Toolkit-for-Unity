# Json Dictionary Utility

## Use Case

Unity's built-in <xref:UnityEngine.JsonUtility> cannot serialize <xref:System.Collections.Generic.Dictionary%602> objects.
Therefore, <xref:i5.Toolkit.Core.Utilities.JsonDictionaryUtility> can be used to serialize and deserialize dictionary data.

## Usage

The usage of <xref:i5.Toolkit.Core.Utilities.JsonDictionaryUtility> is identical to <xref:UnityEngine.JsonUtility>:

### Serialize to JSON

To serialize an array to JSON, call <xref:i5.Toolkit.Core.Utilities.JsonDictionaryUtility.ToJson*>.
It returns a JSON object with two arrays "keys" and "values".
These arrays are the contents of the array entries.

```[C#]
Dictionary<string, int> dictionary = new Dictionary<string, int>();
dictionary.Add("firstKey", 42);
dictionary.Add("secondKey", 1);

string json = JsonDictionaryUtility.ToJson(dictionary);
// result:
// "{\"keys\":[\"firstKey\",\"secondKey\"],\"values\":[42,1]}"
```

### Deserialize from JSON

If the JSON string uses the same format of a "keys"-array and a "values"-array, you can use <xref:i5.Toolkit.Core.Utilities.JsonArrayUtility.FromJson*> to deserialize the JSON string and convert it to a native <xref:System.Collections.Generic.Dictionary%602>.

```[C#]
string json = "{\"keys\":[\"firstKey\",\"secondKey\"],\"values\":[42,1]}";
Dictionary<string, int> dictionary = JsonDictionaryUtility.FromJson<string, int>(json);
// resulting dictionary has two entries with:
// dictionary["firstKey"] == 42
// dictionary["secondKey"] == 1
```

## Functionality

A dictionary consists of a set of key value pairs with unique keys.
<xref:i5.Toolkit.Core.Utilities.JsonDictionaryUtility> reads all keys and serializes them into an JSON array.
The values are also serialized into their own array.
The key-value relationship is preserved by the same indices.
This means that the first enty in the keys array is the key for the first value in the values array and so on.

> <xref:i5.Toolkit.Core.Utilities.JsonDictionaryUtility> cannot serialize or deserialize dictionaries to/from key-value pairs in the JSON string.
> It relies on the unfolding of the entries into the key and value arrays.
> Therefore, it is not directly compatible with other JSON libraries that convert dictionary keys to JSON keys and the dictionary values to the value of a key entry.