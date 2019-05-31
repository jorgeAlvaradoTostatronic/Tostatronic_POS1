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
	if ($resultado = $enlace->query($consulta)) 
	{
	    while ($fila = $resultado->fetch_row()) 
	    {
	    	$encontrado=false;
			foreach ($data as $product) 
			{
				if($product->Codigo==$fila[0])
				{
					$qty=$product->Cantidad;
					$precio=$product->PrecioPublico;
					$encontrado=true;
					break;
				}
			}
			if($encontrado)
			{
				$consulta2 = "UPDATE ps_stock_available, ps_product_shop SET ps_stock_available.quantity=?,ps_product_shop.price=? where ps_stock_available.id_product =? AND ps_product_shop.id_product=? ";
				$sentencia = $enlace->prepare($consulta2);
				$val1 = $qty;
				$val2 = $precio;
				$val3 = $fila[1];
				$val4 = $fila[1];
				$sentencia->bind_param("idii", $val1, $val2,$val3,$val4);
				$sentencia->execute();
				$sentencia->close();
			}
	    }
	    /* liberar el conjunto de resultados */
	    echo "Proceso realizado con exito";
	    $resultado->close();
	}

	mysqli_close($enlace);
?>