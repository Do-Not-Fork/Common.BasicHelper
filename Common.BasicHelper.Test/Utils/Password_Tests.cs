﻿namespace Common.BasicHelper.Utils;

[TestClass]
public class Password_Tests
{
    [TestMethod]
    public void Test_GeneratePassword()
    {
        foreach (var _ in Enumerable.Range(0, 10))
            Console.WriteLine(Password.GeneratePassword(length: 12));
    }
}
