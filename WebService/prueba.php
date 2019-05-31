<?php
	$enlace = mysqli_connect("mysql.tostratonic.com", "tostatronic_symf", "Jorge1995", "tostatronic_symfony");

	if (!$enlace) {
	    echo "Error: No se pudo conectar a MySQL." . PHP_EOL;
	    echo "errno de depuración: " . mysqli_connect_errno() . PHP_EOL;
	    echo "error de depuración: " . mysqli_connect_error() . PHP_EOL;
	    exit;
	}
	$consulta2 = "INSERT INTO `item` (`id`, `item_unit_id`, `item_category_id`, `name`, `code`, `quantity`, `price`, `description`, `active`, `image_description`, `image`, `created`, `updated`) VALUES (NULL, '1', '4', 'jhweg', '5456', '100', '546', 'jhgjhfjhv jkvhv', '1', NULL, 'mbgvvjh.jpg', '2018-09-07 00:00:00', '2018-09-01 00:00:00');";
	if($enlace->query($consulta2)==TRUE)
		echo "Si";
	else
		echo "NO"."<br>".$enlace->error;
	mysqli_close($enlace);
?>