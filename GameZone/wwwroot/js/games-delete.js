$(document).ready(function () {
	$('.js-delete').on('click', function () {
		var id = $(this).data('id');
		var url = '/Games/Delete' + '/' + id;

		const swal = Swal.mixin({
			customClass: {
				confirmButton: 'btn btn-danger mx-2',
				cancelButton: 'btn btn-light'
			},
			buttonsStyling: false
		})
		swal.fire({
			title: 'Delete this game?!!',
			text: "You won't be able to revert this!",
			icon: 'warning',
			showCancelButton: true,
			confirmButtonText: 'Yes, delete it!',
			cancelButtonText: 'No, cancel!',
			reverseButtons: true
		}).then((result) => {
			if (result.isConfirmed) {
				$.ajax({
					url: url,
					type: 'DELETE',
					success: function () {
						swal.fire(
							'Deleted!',
							'Game has been deleted.',
							'success'
						);
						location.reload(true);
					},
					error: function () {
						swal.fire(
							'OoooPS ...',
							'Something went wrong.',
							'error'
						);
					}
				});

			}
		});
		
	});
});