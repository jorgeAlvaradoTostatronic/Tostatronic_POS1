<?php
	$enlace = mysqli_connect("mysql.tostratonic.com", "tostatronic_symf", "Jorge1995", "tostatronic_symfony");

	if (!$enlace) {
	    echo "Error: No se pudo conectar a MySQL." . PHP_EOL;
	    echo "errno de depuración: " . mysqli_connect_errno() . PHP_EOL;
	    echo "error de depuración: " . mysqli_connect_error() . PHP_EOL;
	    exit;
	}
	$data = file_get_contents("php://input",true);
	$data=json_decode($data);
	foreach ($data as $product) 
	{
		$codigo = $product->Codigo;
		$precio = $product->Precio;
		$existencia=$product->Cantidad;
		$consulta2 = "UPDATE item SET item.price=".$precio.", item.quantity=".$existencia." where item.code ='".$codigo."';";
		if($enlace->query($consulta2)==FALSE)
		{
			echo "NO"."<br>".$enlace->error;
		}
	}
    /* liberar el conjunto de resultados */
    echo "Proceso realizado con exito";
    $resultado->close();
	mysqli_close($enlace);
?>