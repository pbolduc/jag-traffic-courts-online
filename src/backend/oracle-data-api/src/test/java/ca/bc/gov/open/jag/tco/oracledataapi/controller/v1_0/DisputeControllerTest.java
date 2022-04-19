package ca.bc.gov.open.jag.tco.oracledataapi.controller.v1_0;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNull;

import java.util.List;

import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Qualifier;

import ca.bc.gov.open.jag.tco.oracledataapi.BaseTestSuite;
import ca.bc.gov.open.jag.tco.oracledataapi.controller.v1_0.DisputeController;
import ca.bc.gov.open.jag.tco.oracledataapi.model.Dispute;
import ca.bc.gov.open.jag.tco.oracledataapi.model.DisputeStatus;
import ca.bc.gov.open.jag.tco.oracledataapi.util.RandomUtil;

class DisputeControllerTest extends BaseTestSuite {

	@Autowired
	@Qualifier("DisputeControllerV1_0")
	private DisputeController disputeController;

	@Test
	public void testSaveDispute() {
		// Assert db is empty and clean
		List<Dispute> allDisputes = disputeController.getAllDisputes();
		assertEquals(0, allDisputes.size());

		// Create a single Dispute
		Dispute dispute = RandomUtil.createDispute();
		Integer disputeId = disputeController.saveDispute(dispute);

		// Assert db contains the single created record
		allDisputes = disputeController.getAllDisputes();
		assertEquals(1, allDisputes.size());
		assertEquals(disputeId, allDisputes.get(0).getId());
		assertEquals(dispute.getDisputantSurname(), allDisputes.get(0).getDisputantSurname());

		// Delete record
		disputeController.deleteDispute(disputeId);

		// Assert db contains is empty again
		allDisputes = disputeController.getAllDisputes();
		assertEquals(0, allDisputes.size());
	}

	@Test
	public void testRejectDispute() {
		// Create a single Dispute
		Dispute dispute = RandomUtil.createDispute();
		Integer disputeId = disputeController.saveDispute(dispute);

		// Retrieve it from the controller's endpoint
		dispute = disputeController.getDispute(disputeId);
		assertEquals(disputeId, dispute.getId());
		assertEquals(DisputeStatus.NEW, dispute.getStatus());

		// Set the status to REJECTED
		disputeController.rejectDispute(disputeId, "Just because");

		// Assert status and reason are set.
		dispute = disputeController.getDispute(disputeId);
		assertEquals(DisputeStatus.REJECTED, dispute.getStatus());
		assertEquals("Just because", dispute.getRejectedReason());
	}

	@Test
	public void testSubmitDispute() {
		// Create a single Dispute
		Dispute dispute = RandomUtil.createDispute();
		Integer disputeId = disputeController.saveDispute(dispute);

		// Retrieve it from the controller's endpoint
		dispute = disputeController.getDispute(disputeId);
		assertEquals(disputeId, dispute.getId());
		assertEquals(DisputeStatus.NEW, dispute.getStatus());

		// Set the status to PROCESSING
		disputeController.submitDispute(disputeId);

		// Assert status is set, rejected reason is NOT set.
		dispute = disputeController.getDispute(disputeId);
		assertEquals(DisputeStatus.PROCESSING, dispute.getStatus());
		assertNull(dispute.getRejectedReason());
	}

	@Test
	public void testCancelDispute() {
		// Create a single Dispute
		Dispute dispute = RandomUtil.createDispute();
		Integer disputeId = disputeController.saveDispute(dispute);

		// Retrieve it from the controller's endpoint
		dispute = disputeController.getDispute(disputeId);
		assertEquals(disputeId, dispute.getId());
		assertEquals(DisputeStatus.NEW, dispute.getStatus());

		// Set the status to CANCELLED
		disputeController.cancelDispute(disputeId);

		// Assert status is set, rejected reason is NOT set.
		dispute = disputeController.getDispute(disputeId);
		assertEquals(DisputeStatus.CANCELLED, dispute.getStatus());
		assertNull(dispute.getRejectedReason());
	}

}
