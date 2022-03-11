using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        var childrenRow = this.Children.NewChildrenRow();
        childrenRow.Id = 1;
        childrenRow.ParentId = 1;
        childrenRow.FirstValue1 = $"FirstValue1.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.FirstValue2 = $"FirstValue2.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.SecondValue1 = $"SecondValue1.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.SecondValue2 = $"SecondValue2.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.Value1Last = $"Value1Last.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.Value2Last = $"Value2Last.{childrenRow.ParentId}.{childrenRow.Id}";
        this.Children.Rows.Add(childrenRow);

        childrenRow = this.Children.NewChildrenRow();
        childrenRow.Id = 2;
        childrenRow.ParentId = 1;
        childrenRow.FirstValue1 = $"FirstValue1.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.FirstValue2 = $"FirstValue2.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.SecondValue1 = $"SecondValue1.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.SecondValue2 = $"SecondValue2.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.Value1Last = $"Value1Last.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.Value2Last = $"Value2Last.{childrenRow.ParentId}.{childrenRow.Id}";
        this.Children.Rows.Add(childrenRow);

        childrenRow = this.Children.NewChildrenRow();
        childrenRow.Id = 3;
        childrenRow.ParentId = 2;
        childrenRow.FirstValue1 = $"FirstValue1.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.FirstValue2 = $"FirstValue2.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.SecondValue1 = $"SecondValue1.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.SecondValue2 = $"SecondValue2.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.Value1Last = $"Value1Last.{childrenRow.ParentId}.{childrenRow.Id}";
        childrenRow.Value2Last = $"Value2Last.{childrenRow.ParentId}.{childrenRow.Id}";
        this.Children.Rows.Add(childrenRow);
    }
}
