import React from 'react';

export function Home() {
  return (
    <div>
      <h1>Hello on this simple CRUD application.</h1>
      <p>Technologies and infrastructure used:</p>
      <ul>
        <li><a href='https://get.asp.net/'>ASP.NET Core</a> and <a href='https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx'>C#</a> for cross-platform server-side code</li>
        <li><a href='https://facebook.github.io/react/'>React</a> for client-side code</li>
        <li><a href='https://www.typescriptlang.org/'>TypeScript</a> for client-side code</li>
        <li><a href='http://getbootstrap.com/'>Bootstrap</a> for layout and styling</li>
        <li><a href='https://azure.microsoft.com/'>Azure</a> infrastructure for hosting an application</li>
        <li><a href='https://www.microsoft.com/en-us/sql-server/'>Microsoft SQL Server</a> for data keeping</li>
        <li><a href='https://www.nuget.org/packages/Swashbuckle/'>Swashbuckle</a> for api documentation generation</li>
      </ul>
    </div>
  );
}