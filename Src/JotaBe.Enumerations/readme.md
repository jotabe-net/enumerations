# Pending tasks

- [ ] Allow ordering comparison of elements - for ex. allow to implement some logic in the abstract class
      for this, or make the TKey IComparable...
- [ ] Document all methods, and the using of the class including:
    - [ ] keeping the ctor non public, to avoid creating spurious instances of the class
    - [ ] the enum elements must be read only static properties
    - [ ] comments for inheriting, can add new properties, and logic, show example of converting temperatures
- [ ] TypeExtensions: could derive 
- [ ] Try if generating the serializer for AOT works fine -- perhaps the type extensions must be
      inside the serializer class
- [ ] The TypeExtensions could use some caching for performance
- [ ] Review test cases, for ex. test cases with converter attribute in property
- [ ] Possible implementation of SerializationMode.RespectEnumMemberStringConverter in Read and Write
- [ ] In converter's read from full object, test and correct the handling of null/missing name/value
- [ ] Modify the docs of MS related to deserializing with naming policies, as done in internal converter, is property
- [ ] Implement the analyzers