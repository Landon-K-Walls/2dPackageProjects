using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCUtil.Procedural
{
  public class LSystem
  {
    string _string;
    string _initialString;
    readonly Dictionary<char, LRule> _ruleMap;

    public string LString => _string;
    public string InitialString => _initialString;
    public LSystem(string baseString, LRule[] rules)
    {
      _ruleMap = rules.ToDictionary(r => r.character);
      _initialString = baseString;
      _string = baseString;
    }

    private string IterateInternal(int times)
    {
      for (int i = 0; i < times; i++)
      {
        StringBuilder builder = new StringBuilder();
        for (int j = 0; j < _string.Length; j++)
        {
          if (_ruleMap.TryGetValue(_string[j], out LRule rule))
            builder.Append(rule.result ?? _string[j].ToString());
          else
            builder.Append(_string[j]);
        }
        _string = builder.ToString();
      }

      return _string;
    }

    public string Iterate(int times) => IterateInternal(times);

    public string Iterate(int times, bool executeRulesOnFinish)
    {

      _string = IterateInternal(times);

      if (executeRulesOnFinish)
        TriggerMatchingRules();


      return _string;
    }

    public LSystem IterateFluent(int times)
    {
      IterateInternal(times);
      return this;
    }
    public LSystem IterateFluent(int times, bool executeRulesOnFinish)
    {
      IterateInternal(times);

      if (executeRulesOnFinish)
        TriggerMatchingRules();

      return this;
    }

    public LSystem IterateFluent(int times, out string lString)
    {
      lString = IterateInternal(times);
      return this;
    }

    public LSystem IterateFluent(int times, out string lString, bool executeRulesOnFinish)
    {
      lString = IterateInternal(times);

      if (executeRulesOnFinish)
        TriggerMatchingRules();

      return this;
    }

    public LSystem AddRule(LRule rule)
    {
      _ruleMap[rule.character] = rule;
      return this;
    }

    public LSystem TriggerMatchingRules()
    {
      foreach (char c in _string)
      {
        if (_ruleMap.TryGetValue(c, out LRule rule))
          rule.ExecuteOnMatch();
      }

      return this;
    }

    public LSystem Reset()
    {
      _string = _initialString;
      return this;
    }
  }

  public record LRule
  {
    public char character;
    public string result;

    public Action OnMatch;

    public LRule(char character, string equalsString, Action func)
    {
      this.character = character;
      result = equalsString;
      OnMatch = func;
    }

    public LRule(char character, string equalsString)
    {
      this.character = character;
      result = equalsString;
      OnMatch = null;
    }

    public LRule(char character, Action func)
    {
      this.character = character;
      result = null;
      OnMatch = func;
    }

    public void ExecuteOnMatch() => OnMatch?.Invoke();
  }
}
