﻿using System.Collections.Generic;
using ErosScriptingEngine.Component;
using ErosScriptingEngine.Parse.Event;
using ErosScriptingEngine.Parse.Expression;
using ErosScriptingEngine.Parse.Statement;

namespace ErosScriptingEngine.Parse
{
    public class ErosExecutableScript : ErosScriptingIOComponent<ErosExecutableScript>
    {
        public readonly List<StatementNode> Statements;
        public readonly ErosScriptStartEvent StartEvent;
        public readonly ErosScriptUpdateEvent UpdateEvent;

        public ErosExecutableScript(List<StatementNode> statements, ErosScriptStartEvent startEvent,
            ErosScriptUpdateEvent updateEvent)
        {
            Statements = statements;
            StartEvent = startEvent;
            UpdateEvent = updateEvent;
        }

        public override object Clone()
        {
            return new ErosExecutableScript(Statements, StartEvent, UpdateEvent);
        }
    }
}