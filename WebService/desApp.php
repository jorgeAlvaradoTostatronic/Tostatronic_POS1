<?php
	require_once('class.html2text.inc');
	$enlace = mysqli_connect("mysql.tostratonic.com", "tstk_jorge", "Jorge1995", "tstk");

	if (!$enlace) {
	    echo "Error: No se pudo conectar a MySQL." . PHP_EOL;
	    echo "errno de depuración: " . mysqli_connect_errno() . PHP_EOL;
	    echo "error de depuración: " . mysqli_connect_error() . PHP_EOL;
	    exit;
	}
	$consulta = "SELECT tstk.ps_product.id_product, tstk.ps_product.reference,tstk.ps_product.id_category_default, tstk.ps_product_lang.link_rewrite ,tstk.ps_product_lang.description, tstk.ps_category_lang.link_rewrite FROM tstk.ps_product, tstk.ps_product_lang, tstk.ps_category_lang WHERE tstk.ps_product_lang.id_product=tstk.ps_product.id_product AND tstk.ps_category_lang.id_category=tstk.ps_product.id_category_default GROUP BY tstk.ps_product.id_product";
		if ($resultado = $enlace->query($consulta)) 
		{
			while ($fila = $resultado->fetch_row()) 
			{
				echo $fila[1];
				echo ' && ';
				echo $fila[2];
				echo ' && ';
				echo $fila[4];
				echo '\n';
			}
		}
	    $resultado->close();

	    /* liberar el conjunto de resultados */
	mysqli_close($enlace);
?>