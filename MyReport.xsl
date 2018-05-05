<?xml version="1.0" encoding="ISO-8859-1"?>

<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
  <html>
  <body>
  <h2>MyPet - Folder/File monitoring Report</h2>
  <table border="1">
    <tr bgcolor="#9acd32">
      <th>Action</th>
      <th>Folder/File Name</th>
      <th>Date and Time</th>
      <th>Computer Name</th>
      <th>UserName</th>
    </tr>
    
    <tr>
      <td bgcolor="#9acd32">
        <b>Created</b> </td>
      <td bgcolor="#9acd32"></td>
      <td bgcolor="#9acd32"></td>
      <td bgcolor="#9acd32"></td>
      <td bgcolor="#9acd32"></td>
    </tr>
    <xsl:for-each select="Report/PartReport[Action = 'Created']">
      <tr>

        <td>
          <xsl:value-of select="Action" />
        </td>
        <td>
          <xsl:value-of select="FolderFileName"/>
        </td>
        <td>
          <xsl:value-of select="DateAndTime"/>
        </td>
        <td>
          <xsl:value-of select="ComputerName"/>
        </td>
        <td>
          <xsl:value-of select="UserName"/>
        </td>
      </tr>
    </xsl:for-each>

    <tr>
      <td bgcolor="#ff0000" >
        <b>Deleted </b>
      </td>
      <td bgcolor="#ff0000"></td>
      <td bgcolor="#ff0000"></td>
      <td bgcolor="#ff0000"></td>
      <td bgcolor="#ff0000"></td>
    </tr>

    <xsl:for-each select="Report/PartReport[Action = 'Deleted']">
      <tr>

        <td>
          <xsl:value-of select="Action" />
        </td>
        <td>
          <xsl:value-of select="FolderFileName"/>
        </td>
        <td>
          <xsl:value-of select="DateAndTime"/>
        </td>
        <td>
          <xsl:value-of select="ComputerName"/>
        </td>
        <td>
          <xsl:value-of select="UserName"/>
        </td>
      </tr>
    </xsl:for-each>


    <tr>
      <td bgcolor="#9acd32">
        <b>Renamed</b>
      </td>
      <td bgcolor="#9acd32"></td>
      <td bgcolor="#9acd32"></td>
      <td bgcolor="#9acd32"></td>
      <td bgcolor="#9acd32"></td>
    </tr>
    <xsl:for-each select="Report/PartReport[Action = 'Renamed']">
      <tr>

        <td>
          <xsl:value-of select="Action" />
        </td>
        <td>
          <xsl:value-of select="FolderFileName"/>
        </td>
        <td>
          <xsl:value-of select="DateAndTime"/>
        </td>
        <td>
          <xsl:value-of select="ComputerName"/>
        </td>
        <td>
          <xsl:value-of select="UserName"/>
        </td>
      </tr>
    </xsl:for-each>
   
    
    <tr>
      <td bgcolor="#9acd32" >
        <b>Changed</b>
    </td>
      <td bgcolor="#9acd32"></td>
      <td bgcolor="#9acd32"></td>
      <td bgcolor="#9acd32"></td>
      <td bgcolor="#9acd32"></td>
    </tr>
    <xsl:for-each select="Report/PartReport[Action = 'Changed']">
      <tr>
        <td>
          <xsl:value-of select="Action" />
        </td>
        <td>
          <xsl:value-of select="FolderFileName"/>
        </td>
        <td>
          <xsl:value-of select="DateAndTime"/>
        </td>
        <td>
          <xsl:value-of select="ComputerName"/>
        </td>
        <td>
          <xsl:value-of select="UserName"/>
        </td>
      </tr>
    </xsl:for-each>
        
  </table>
  </body>
  </html>
</xsl:template>

</xsl:stylesheet>