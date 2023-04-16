namespace PiraSoft.Tools.UnitTest.Data.Mapping;

public partial class TestDataSet
{
    public void Popolate()
    {
        var categoryRow = this.Categories.NewCategoriesRow();
        categoryRow.Code = "Code1";
        categoryRow.Value = 2;
        this.Categories.Rows.Add(categoryRow);

        var parentRow = this.Parents.NewParentsRow();
        parentRow.Id = 1;
        parentRow.Name = "Name1";
        parentRow.CategoryCode = "Code1";
        this.Parents.Rows.Add(parentRow);

        parentRow = this.Parents.NewParentsRow();
        parentRow.Id = 2;
        parentRow.Name = "Name2";
        this.Parents.Rows.Add(parentRow);

        parentRow = this.Parents.NewParentsRow();
        parentRow.Id = 3;
        parentRow.Name = "Name3";
        this.Parents.Rows.Add(parentRow);

        var childrenRow = this.Children.NewChildrenRow();
        childrenRow.Id = 1;
        childrenRow.ParentId = 1;
        childrenRow.Description = $"{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.Value1 = $"Value1.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.Value2 = $"Value2.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.Value3 = $"Value3.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.PrefixValue1 = $"PrefixValue1.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.PrefixValue2 = $"PrefixValue2.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.PrefixValue3 = $"PrefixValue3.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.Value1Suffix = $"Value1Suffix.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.Value2Suffix = $"Value2Suffix.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.Value3Suffix = $"Value3Suffix.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.PrefixValue1Suffix = $"PrefixValue1Suffix.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.PrefixValue2Suffix = $"PrefixValue2Suffix.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.PrefixValue3Suffix = $"PrefixValue3Suffix.{childrenRow.ParentId}.{childrenRow.Id}";
        this.Children.Rows.Add(childrenRow);

        childrenRow = this.Children.NewChildrenRow();
        childrenRow.Id = 2;
        childrenRow.ParentId = 1;
        childrenRow.Description = $"{childrenRow.ParentId}.{childrenRow.Id}";

        childrenRow.PrefixValue3 = $"PrefixValue3.{childrenRow.ParentId}.{childrenRow.Id}";

        childrenRow.Value1Suffix = $"Value1Suffix.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.Value3Suffix = $"Value3Suffix.{childrenRow.ParentId}.{childrenRow.Id}";

        childrenRow.PrefixValue1Suffix = $"PrefixValue1Suffix.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.PrefixValue2Suffix = $"PrefixValue2Suffix.{childrenRow.ParentId}.{childrenRow.Id}";

        this.Children.Rows.Add(childrenRow);

        childrenRow = this.Children.NewChildrenRow();
        childrenRow.Id = 3;
        childrenRow.ParentId = 2;
        childrenRow.Description = $"{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.Value1 = $"Value1.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.Value2 = $"Value2.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.Value2 = $"Value3.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.PrefixValue1 = $"PrefixValue1.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.PrefixValue2 = $"PrefixValue2.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.PrefixValue2 = $"PrefixValue3.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.Value1Suffix = $"Value1Suffix.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.Value2Suffix = $"Value2Suffix.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.Value2Suffix = $"Value3Suffix.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.PrefixValue1Suffix = $"PrefixValue1Suffix.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.PrefixValue2Suffix = $"PrefixValue2Suffix.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.PrefixValue2Suffix = $"PrefixValue3Suffix.{childrenRow.ParentId}.{childrenRow.Id}";
        this.Children.Rows.Add(childrenRow);
    }
}
