<?php
	$enlace = mysqli_connect("mysql.tostratonic.com", "tstk_jorge", "Jorge1995", "tstk");

	if (!$enlace) {
	    echo "Error: No se pudo conectar a MySQL." . PHP_EOL;
	    echo "errno de depuración: " . mysqli_connect_errno() . PHP_EOL;
	    echo "error de depuración: " . mysqli_connect_error() . PHP_EOL;
	    exit;
	}
	$consulta = "SELECT tstk.ps_product.reference, tstk.ps_product.id_product FROM tstk.ps_product";
	$data = file_get_contents("php://input",true);
	$data=json_decode($data);
	$qty=0;
	$precio=0;
	$encontrado=true;
	
	foreach ($data as $product) 
	{
		$encontrado=true;
		if ($resultado = $enlace->query($consulta)) 
		{
			while ($fila = $resultado->fetch_row()) 
			{
				if($product->Codigo==$fila[0])
				{
					$encontrado=false;
					break;
				}
			}
			if($encontrado)
			{
				echo $product->Nombre;
				echo '  &&  ';
			}
		}
	    $resultado->close();
	}

	    /* liberar el conjunto de resultados */
	    echo "Proceso realizado con exito";
	mysqli_close($enlace);
?>