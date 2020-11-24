# Json Array Utility

## Use Case

Unity's built-in <xref:UnityEngine.JsonUtility> does not support JSON strings which have an array at root level:
To solve this, the <xref:i5.Toolkit.Core.Utilities.JsonArrayUtility> was implemented.

<xref:i5.Toolkit.Core.Utilities.JsonArrayUtility> is e.g. useful when communicating with a Web API that returns list of objects for a query.

<xref:i5.Toolkit.Core.Utilities.JsonArrayUtility> is only required if the JSON array is on the root level of the JSON string.
If it is part of a JSON object, Unity's <xref:UnityEngine.JsonUtility> can handle it.

## Usage

The usage of <xref:i5.Toolkit.Core.Utilities.JsonArrayUtility> is identical to <xref:UnityEngine.JsonUtility>:

### Serialize to JSON

To serialize an array to JSON, call <xref:i5.Toolkit.Core.Utilities.JsonArrayUtility.ToJson*>.
It returns a JSON object with one key "array".
The value of this key is the array.

```[C#]
int[] intArray = { 1, 2, 3, 4, 5 };
string serializedJson = JsonArrayUtility.ToJson(intArray);
// result is:
// {"array":[1,2,3,4,5]}
```

### Deserialize from JSON

If the JSON string already has the from where the array is encapuslated into the "array" key-value pair, you can use <xref:i5.Toolkit.Core.Utilities.JsonArrayUtility.FromJson*> to deserialize and unwrap the array.

```[C#]
string wrappedArray = "{\"array\":[1,2,3,4,5]}";
int[] deserializedArray = JsonArrayUtility.FromJson<int>(wrappedArray);
```

If the JSON string is not encapsulated but has the array on its root level, first call <xref:i5.Toolkit.Core.Utilities.JsonArrayUtility.EncapsulateInWrapper*> and then <xref:i5.Toolkit.Core.Utilities.JsonArrayUtility.FromJson*>.

```[C#]
string jsonArray = "[1,2,3,4,5]";
string wrappedArray = JsonArrayUtility.EncapsulateInWrapper(jsonArray);
int[] deserializedArray = JsonArrayUtility.FromJson<int>(wrappedArray);
```

## Functionality

<xref:i5.Toolkit.Core.Utilities.JsonArrayUtility> wraps arrays into an object so that the array is not at root level anymore.
After that, it uses Unity's <xref:UnityEngine.JsonUtility> to serialize the JSON string.

To deserialize, <xref:i5.Toolkit.Core.Utilities.JsonArrayUtility.EncapsulateInWrapper*> adds the JSON object wrapper around the JSON string.
The <xref:i5.Toolkit.Core.Utilities.JsonArrayUtility.FromJson*> deserializes the wrapped JSON string to the wrapper object.
After that, it unpacks the wrapper object and returns the contained array.