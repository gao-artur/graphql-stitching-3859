using System.Collections.Generic;
using HotChocolate.Types;

namespace Remote
{
    public class Query : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("Query");
            descriptor.Field("hot").Resolve("chocolate");
        }
    }

    public class Mutation : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor
                .Field("do")
                .Argument("input", a => a.Type<MyInputType>())
                .Type<MyOutputType>()
                .Resolve(context =>
                {
                    var input = context.ArgumentValue<MyInput>("input");
                    return new MyOutput
                    {
                        Params = input.Params,
                    };
                });
        }
    }

    public class MyInput
    {
        public IDictionary<string, object> Params { get; set; }
    }

    public class MyOutputType : ObjectType<MyOutput>
    {
        protected override void Configure(IObjectTypeDescriptor<MyOutput> descriptor)
        {
            descriptor.Field(output => output.Params).Type<PropertiesDictionaryType>();
        }
    }

    public class MyOutput
    {
        public IDictionary<string, object> Params { get; set; }
    }

    public class MyInputType : InputObjectType<MyInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<MyInput> descriptor)
        {
            descriptor.Field(input => input.Params).Type<PropertiesDictionaryType>();
        }
    }
}