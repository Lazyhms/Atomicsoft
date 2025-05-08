using System.ComponentModel;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Xunit;

namespace AspNetCore.XUnit;

public class UnitTest1
{
    private readonly JsonNode _node = JsonNode.Parse(JsonSerializer.Serialize(new { Id = 1, Name = "我的" }), new JsonNodeOptions { PropertyNameCaseInsensitive = true })!;

    [Fact]
    public void Test1()
    {



        var jsonArray = JsonSerializer.SerializeToNode(new object[] { new { id = 1, name = "2" }, new { id = 2, name = "3" } });

        var tttt = jsonArray!.AsArray().OfType<JsonObject>().Select(s => s.GetPropertyValue<string>("name"));

        var jsonObject = JsonSerializer.SerializeToNode(new { Id = 1, Name = "222" })!.AsObject();
        jsonObject.AddOrUpdate("name", "333");
        jsonObject.TryUpdate("ddd", "122");

        Expression<Func<Test, bool>> expression = t => true;
        Expression<Func<Test, bool>> expression1 = t => false;
        var expression2 = expression.And(x => x.Id == 1);
        var expression3 = expression1.And(x => x.Id == 1);

        var expression4 = expression.Or(x => x.Id == 1);
        var expression5 = expression1.Or(x => x.Id == 1);

        var t = Enumerable.Range(0, 2).Insert(1, 4);

        var t1 = Enumerable.Range(0, 2).Insert(1, Enumerable.Range(4, 3));

        var tt2 = new List<Test> { new() { Id = 1, }, new() { Id = 2, } }.GroupBy(g => g.Id, (r1, r2) => new
        {
            r1,
            Name = r2.Select(s => s.Name)
        });

        var tt3 = new List<Test> { new() { Id = 1, }, new() { Id = 2, }, new() { Id = 1, } }.LastIndexOf(f => f.Id == 1);

        int? i = 1;

        BizException.ThrowIfNull(i, "不能为空");

        var node = _node.AsObject();

        var value = node["Id"]?.AsValue();
    }

    [Fact]
    public void Test3()
    {
        var options = JsonSerializerOptions.Default.ApplyWebDefault();
        //options.TypeInfoResolver = new MultipleJsonPropertyNameResolver();

        var i1 = JsonSerializer.Deserialize<Tes>(@"{""Date"":""""}", options);

        var ttt1 = JsonSerializer.Serialize(new Test { Score = 0.00001m }, options);
        var ttt22222 = JsonSerializer.Deserialize<Test>(@"{
  ""aid"": ""120"",
  ""pId"": ""0"",
  ""score"": ""0.00001"",
  ""name"": """",
  ""guid"": ""(00000000-0000-0000-0000-000000000000)""
}", options);

        var t1 = Tests.A.GetDescription();
        var t3 = Tests.A.GetDescription();

        var t2 = Enum.GetNames<Tests>();

        Test[] t = [new Test { Id = 1, PId = 0, Name = "11" }, new Test { Id = 2, PId = 1, Name = "22" }, new Test { Id = 3, PId = 1, Name = "33" }, new Test { Id = 4, PId = 3, Name = "22" }];
        var tt = t.ToTreeNode(s => s.Id, p => p.PId).ToList();
        var ttt2 = t.ToTreeNode(s => s.Id, p => p.PId).FilterNode(f => f.Name == "33").ToList();

        var ttt = JsonSerializer.Serialize(tt, options);
    }
}

public class Tes
{
    public int? Id { get; set; }

    public DateTime? Date { get; set; }
}

public class Test
{
    [JsonPropertyNames("aid", "cId", "did")]
    public int Id { get; set; }

    public int PId { get; set; }

    public decimal Score { get; set; }

    public string? Name { get; set; } = null;

    [JsonGuidHandling(JsonGuidHandling.Parentheses)]
    public Guid Guid { get; set; }

    public Tests Tests { get; set; } = Tests.A;
}


public enum Tests
{
    [Description("wo s A")]
    A,

    B, C, D, E, F,
}
