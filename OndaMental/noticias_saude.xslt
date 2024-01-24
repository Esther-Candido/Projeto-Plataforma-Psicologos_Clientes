<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="xml" indent="yes"/>

	<xsl:template match="/">
		<div class="container mt-5" style="margin-top: -40px!important;">
			<div style="background-color: #f8f9fa; padding-bottom: 20px; text-align: center; margin-top: 20px;">
				<h2 class="display-4 text-center" style="font-family: 'Pacifico', cursive; color: #747681; padding-block: 10px; margin-bottom: 0px!important;">Viva Bem: Inspirações para seu Estilo de Vida</h2>
			</div>

			<div class="row">
				<xsl:for-each select="/rss/channel/item[category='Lifestyle'][position() &lt; 4]"> <!--Ira mostrar 3 noticias -->
					<div class="col-md-4 mb-4">
						<div class="card h-100">
							<a href="{link}" target="_blank">
								<img class="card-img-top" src="{enclosure/@url}" alt="{title}" />
							</a>
							<div class="card-body">
								<h5 class="card-title">
									<xsl:value-of select="title" />
								</h5>
								<p class="card-text">
									<xsl:value-of select="description" />
								</p>
							</div>
						</div>
					</div>
				</xsl:for-each>
			</div>
		</div>

	


	</xsl:template>
</xsl:stylesheet>