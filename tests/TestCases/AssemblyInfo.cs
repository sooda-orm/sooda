//
// Copyright (c) 2003-2006 Jaroslaw Kowalski <jaak@jkowalski.net>
// Copyright (c) 2006-2014 Piotr Fusik <piotr@fusik.info>
//
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:
//
// * Redistributions of source code must retain the above copyright notice,
//   this list of conditions and the following disclaimer.
//
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
// THE POSSIBILITY OF SUCH DAMAGE.
//

using System;
using System.Reflection;
using Sooda;
using Sooda.Schema;
using Sooda.UnitTests.TestCases;

[assembly: AssemblyTitle("Sooda.Utils")]
[assembly: AssemblyDescription("Application framework for Sooda")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Sooda Community - www.sooda.org")]
[assembly: AssemblyProduct("Sooda - Simple Object Oriented Database Access")]
[assembly: AssemblyCopyright("Copyright (c) 2002-2005 by Jaroslaw Kowalski")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyVersion("0.7.0.0")]

[assembly: InjectDependentSchemas]
[assembly: SoodaStubAssembly(typeof(Sooda.Schema.MultiAssemblySchema))]
[assembly: SoodaConfig(XmlConfigFileName = "Sooda.config.xml")]


